import { test, expect } from "@playwright/test";
import { BASE_URL } from "../src/constants/base-url.constant";

test.describe("Homepage", () => {
  test("should load correctly", async ({ page }) => {
    await page.goto(BASE_URL); // Adjust URL as needed

    // Check for the title
    const title = await page.title();
    expect(title).toContain("StateNet");

    // Verify the main content title is visible
    await expect(
      page.locator("text=State machines easier, faster and more reliable")
    ).toBeVisible();
  });
});
