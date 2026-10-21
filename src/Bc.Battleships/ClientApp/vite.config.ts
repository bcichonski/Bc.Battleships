/// <reference types="vitest/config" />
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

const apiTarget = process.env.ASPNETCORE_URLS?.split(';')[0] || 'http://localhost:5191'

export default defineConfig({
  plugins: [react()],
  // Optional override (e.g. when node_modules sits on a filesystem that rejects directory renames).
  cacheDir: process.env.VITE_CACHE_DIR || undefined,
  build: {
    outDir: 'build',
  },
  server: {
    port: 44463,
    strictPort: true,
    proxy: {
      '/game': {
        target: apiTarget,
        secure: false,
      },
    },
  },
  test: {
    environment: 'jsdom',
    setupFiles: ['./src/setupTests.ts'],
    include: ['src/**/*.test.{ts,tsx}'],
    globals: false,
  },
})
