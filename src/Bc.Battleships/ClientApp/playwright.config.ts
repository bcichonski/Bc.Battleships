import { mkdtempSync } from 'node:fs'
import { tmpdir } from 'node:os'
import { join } from 'node:path'
import { defineConfig, devices } from '@playwright/test'

const backendUrl = 'http://127.0.0.1:5191'
const clientUrl = 'http://localhost:44463'

// Throw-away database so e2e never touches the developer's battleships.db.
const dbDir = mkdtempSync(join(tmpdir(), 'battleships-e2e-'))
const connectionString = `Data Source=${join(dbDir, 'e2e.db')}`

export default defineConfig({
  testDir: './e2e',
  fullyParallel: false,
  workers: 1,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 0,
  reporter: [['list'], ['html', { open: 'never' }]],
  use: {
    baseURL: clientUrl,
    trace: 'on-first-retry',
  },
  projects: [{ name: 'chromium', use: { ...devices['Desktop Chrome'] } }],
  webServer: [
    {
      command: `dotnet run --project ../Bc.Battleships.csproj --no-launch-profile --urls ${backendUrl}`,
      // The API has no health URL, so wait for the port.
      port: 5191,
      reuseExistingServer: !process.env.CI,
      timeout: 180_000,
      env: {
        ASPNETCORE_ENVIRONMENT: 'Development',
        ConnectionString: connectionString,
      },
    },
    {
      command: 'npm run dev',
      url: clientUrl,
      reuseExistingServer: !process.env.CI,
      timeout: 60_000,
      env: { ASPNETCORE_URLS: backendUrl },
    },
  ],
})
