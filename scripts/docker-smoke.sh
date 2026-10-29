#!/usr/bin/env bash
# Smoke test for the Docker image: builds and starts it with docker compose, creates a game,
# fires a shot, restarts the container and verifies the game survived on the data volume.
#
# Usage: bash scripts/docker-smoke.sh
# Env:   BASE_URL         base address of the app (default http://localhost:8080)
#        HEALTH_TIMEOUT   seconds to wait for the container to become healthy (default 120)
# Exits non-zero on the first failed assertion. Always tears down compose (including the volume).
set -euo pipefail

cd "$(dirname "$0")/.."

BASE_URL="${BASE_URL:-http://localhost:8080}"
HEALTH_TIMEOUT="${HEALTH_TIMEOUT:-120}"

cleanup() {
  docker compose down -v --remove-orphans >/dev/null 2>&1 || true
}
trap cleanup EXIT

fail() {
  echo "FAIL: $*" >&2
  docker compose logs --tail 50 >&2 || true
  exit 1
}

wait_healthy() {
  local cid status
  for ((i = 0; i < HEALTH_TIMEOUT; i++)); do
    cid=$(docker compose ps -q battleships)
    if [[ -n "$cid" ]]; then
      status=$(docker inspect -f '{{if .State.Health}}{{.State.Health.Status}}{{else}}none{{end}}' "$cid")
      [[ "$status" == "healthy" ]] && return 0
    fi
    sleep 1
  done
  fail "container did not become healthy within ${HEALTH_TIMEOUT}s"
}

docker compose down -v --remove-orphans >/dev/null 2>&1 || true
docker compose up -d --build
wait_healthy
echo "Container healthy"

# Plain HTTP must be served directly, not redirected to HTTPS
code=$(curl -s -o /dev/null -w '%{http_code}' "$BASE_URL/healthz")
[[ "$code" == "200" ]] || fail "GET /healthz returned $code (expected 200, no redirect)"

# Create a game
created=$(curl -sf -X POST "$BASE_URL/games") || fail "POST /games failed"
GAME_ID=$(echo "$created" | jq -r '.id')
[[ -n "$GAME_ID" && "$GAME_ID" != "null" ]] || fail "POST /games returned no id: $created"
echo "Created game $GAME_ID"

# Fire a shot
shot_code=$(curl -s -o /dev/null -w '%{http_code}' -X POST "$BASE_URL/games/$GAME_ID/shots" \
  -H 'Content-Type: application/json' -d '{"column":0,"row":0}')
[[ "$shot_code" =~ ^2 ]] || fail "POST shot returned $shot_code"
echo "Shot accepted"

# Restart and verify persistence
docker compose restart
wait_healthy
got=$(curl -sf "$BASE_URL/games/$GAME_ID") || fail "GET /games/$GAME_ID failed after restart"
[[ "$(echo "$got" | jq -r '.id')" == "$GAME_ID" ]] || fail "game id mismatch after restart: $got"
echo "Game survived restart"

# Non-root
uid=$(docker compose exec -T battleships id -u | tr -d '\r')
[[ "$uid" != "0" ]] || fail "container runs as root"
echo "Running as uid $uid"

echo "Docker smoke test passed"
