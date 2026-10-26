#!/usr/bin/env bash
# Runs the test suite with code coverage, builds the filtered coverage report and
# fails when line or branch coverage is below the thresholds.
#
# Usage: bash scripts/coverage.sh [extra dotnet test args, e.g. -c Release --no-build]
# Env:   COVERAGE_MIN_LINE   minimum filtered line coverage in % (default 90)
#        COVERAGE_MIN_BRANCH minimum filtered branch coverage in % (default 85)
set -euo pipefail

cd "$(dirname "$0")/.."

MIN_LINE="${COVERAGE_MIN_LINE:-90}"
MIN_BRANCH="${COVERAGE_MIN_BRANCH:-85}"

rm -rf TestResults CoverageReport

dotnet test Bc.Battleships.slnx --collect "XPlat Code Coverage" --results-directory ./TestResults "$@"

reportgenerator \
  -reports:"TestResults/**/coverage.cobertura.xml" \
  -targetdir:"CoverageReport" \
  -reporttypes:"Html;MarkdownSummaryGithub;JsonSummary" \
  -assemblyfilters:"-Bc.Battleships.Tests.*;-Bc.Battleships.UnitTests.*" \
  -classfilters:"-Mediator.*;-Microsoft.*;-System.*;-*.Migrations.*"

SUMMARY="CoverageReport/Summary.json"
if command -v jq >/dev/null 2>&1; then
  LINE=$(jq -r '.summary.linecoverage' "$SUMMARY")
  BRANCH=$(jq -r '.summary.branchcoverage' "$SUMMARY")
else
  LINE=$(python3 -c 'import json,sys; print(json.load(open(sys.argv[1]))["summary"]["linecoverage"])' "$SUMMARY")
  BRANCH=$(python3 -c 'import json,sys; print(json.load(open(sys.argv[1]))["summary"]["branchcoverage"])' "$SUMMARY")
fi

echo "Coverage (filtered): line ${LINE}% (minimum ${MIN_LINE}%), branch ${BRANCH}% (minimum ${MIN_BRANCH}%)"

FAILED=0
if ! awk -v v="$LINE" -v m="$MIN_LINE" 'BEGIN { exit !(v + 0 >= m + 0) }'; then
  echo "Coverage gate FAILED: line coverage ${LINE}% is below ${MIN_LINE}%" >&2
  FAILED=1
fi
if ! awk -v v="$BRANCH" -v m="$MIN_BRANCH" 'BEGIN { exit !(v + 0 >= m + 0) }'; then
  echo "Coverage gate FAILED: branch coverage ${BRANCH}% is below ${MIN_BRANCH}%" >&2
  FAILED=1
fi

if [ "$FAILED" -ne 0 ]; then
  exit 1
fi

echo "Coverage gate passed."
