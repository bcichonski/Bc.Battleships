#!/usr/bin/env bash
# Runs the test suite with code coverage, builds the filtered coverage report and
# warns (without failing) when line or branch coverage is below the thresholds.
#
# Usage: bash scripts/coverage.sh [extra dotnet test args, e.g. -c Release --no-build]
# Env:   COVERAGE_MIN_LINE   warning threshold for filtered line coverage in % (default 90)
#        COVERAGE_MIN_BRANCH warning threshold for filtered branch coverage in % (default 85)
# Exits non-zero only when the tests or the report generation fail, never on low coverage.
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

echo "Coverage (filtered): line ${LINE}% (warning below ${MIN_LINE}%), branch ${BRANCH}% (warning below ${MIN_BRANCH}%)"

# Low coverage is a warning, not a failure. On GitHub Actions it is also shown as a workflow annotation.
warn() {
  echo "Coverage warning: $1" >&2
  if [ -n "${GITHUB_ACTIONS:-}" ]; then
    echo "::warning title=Coverage below threshold::$1"
  fi
}

BELOW=0
if ! awk -v v="$LINE" -v m="$MIN_LINE" 'BEGIN { exit !(v + 0 >= m + 0) }'; then
  warn "line coverage ${LINE}% is below ${MIN_LINE}%"
  BELOW=1
fi
if ! awk -v v="$BRANCH" -v m="$MIN_BRANCH" 'BEGIN { exit !(v + 0 >= m + 0) }'; then
  warn "branch coverage ${BRANCH}% is below ${MIN_BRANCH}%"
  BELOW=1
fi

if [ "$BELOW" -eq 0 ]; then
  echo "Coverage is at or above the thresholds."
fi
