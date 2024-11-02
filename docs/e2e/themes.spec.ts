import { test, expect, Page } from "@playwright/test";
import { BASE_URL } from "../src/constants/base-url.constant";

const getThemeVar = async (page: Page) =>
  await page.evaluate(() => {
    return getComputedStyle(document.documentElement)
      .getPropertyValue("--ifm-color-scheme")
      .trim();
  });

const getSwitch = (page: Page) =>
  page.locator('[aria-label*="Switch between dark and light mode"]');

test("switch light to dark theme", async ({ page }) => {
  await page.goto(BASE_URL);

  const element = getSwitch(page);
  expect(await getThemeVar(page)).toEqual("light");

  await element.click();
  await page.waitForTimeout(100);

  expect(await getThemeVar(page)).toEqual("dark");
});

test("switch dark to light theme", async ({ page }) => {
  await page.addInitScript(() => {
    localStorage.setItem("theme", "dark");
  });
  await page.goto(BASE_URL);
  const element = getSwitch(page);
  expect(await getThemeVar(page)).toEqual("dark");

  await element.click();
  await page.waitForTimeout(100);

  expect(await getThemeVar(page)).toEqual("light");
});
