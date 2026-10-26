import { expect, test } from '@playwright/test'

const ROWS = 'ABCDEFGHIJ'
const COLUMNS = 10

const hitPoints = async (text: string | null) => Number(/(\d+)/.exec(text ?? '')?.[1])

test('plays a full game until "You win!"', async ({ page }) => {
  await page.goto('/')

  await page.getByRole('navigation').getByRole('button', { name: 'Start new game' }).click()
  const status = page.getByText(/Left to hit:/)
  await expect(status).toBeVisible()
  const initialHp = await hitPoints(await status.textContent())
  expect(initialHp).toBeGreaterThan(0)

  const cells = ROWS.split('').flatMap((row) =>
    Array.from({ length: COLUMNS }, (_, column) => `${row}${column + 1}`),
  )
  const win = page.getByText('You win!')
  let shots = 0

  for (const name of cells) {
    if (await win.isVisible()) break
    const cell = page.getByRole('button', { name, exact: true })
    await cell.click()
    shots++
    // Wait for the shot result: the cell shows a symbol, or the game is over.
    await expect(cell.or(win)).toHaveText(/[●○]|You win!/)

    if (shots === 1 && !(await win.isVisible())) {
      const symbol = (await cell.textContent()) ?? ''
      const hp = await hitPoints(await status.textContent())
      expect(symbol).toMatch(/[●○]/)
      expect(hp).toBe(symbol === '●' ? initialHp - 1 : initialHp)
    }
  }

  await expect(win).toBeVisible()
  expect(shots).toBeLessThanOrEqual(100)
})

test('reload mid-game resumes the same game', async ({ page }) => {
  await page.goto('/')
  await page.getByRole('navigation').getByRole('button', { name: 'Start new game' }).click()
  const status = page.getByText(/Left to hit:/)
  await expect(status).toBeVisible()
  const initialHp = await hitPoints(await status.textContent())

  // Shoot until a hit lowers the hit points, so a reload that started a fresh game would be noticed.
  let shot = ''
  for (const row of ROWS) {
    for (let column = 1; column <= COLUMNS; column++) {
      const cell = page.getByRole('button', { name: `${row}${column}`, exact: true })
      await cell.click()
      await expect(cell).toHaveText(/[●○]/)
      if ((await cell.textContent()) === '●') {
        shot = `${row}${column}`
        break
      }
    }
    if (shot) break
  }
  expect(shot).not.toBe('')
  await expect(status).toHaveText(`Left to hit: ${initialHp - 1}`)

  await page.reload()

  await expect(page.getByText(/Left to hit:/)).toHaveText(`Left to hit: ${initialHp - 1}`)
  await expect(page.getByRole('button', { name: shot, exact: true })).toHaveText('●')
})
