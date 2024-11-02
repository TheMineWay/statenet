import { test, expect } from "@playwright/test";
import { BASE_URL } from "../src/constants/base-url.constant";

test.describe("Navigate from", () => {
  test.describe("home to", () => {
    test.beforeEach(async ({ page }) => await page.goto(BASE_URL));

    test("the blog entries list", async ({ page }) => {
      // Click on the Blog link
      await page.click("text=Blog");

      // Confirm that the blog list is visible
      await expect(page.locator("text=Recent posts")).toBeVisible();
    });

    test("the tutorial", async ({ page }) => {
      // Click on the Tutorial link
      await page.click("text=Docs");

      // Confirm that the title is visible
      await expect(page.locator('h1:has-text("Introduction")')).toBeVisible();
    });
  });

  test.describe("blog to", () => {
    test.beforeEach(async ({ page }) => await page.goto(BASE_URL + "/blog"));

    test("the home", async ({ page }) => {
      const homeBtn = page.locator('.navbar__title:has-text("StateNet")');
      await homeBtn.click();

      await page.waitForLoadState("load");

      expect(page.url()).toEqual(BASE_URL + "/");
    });
  });
});
