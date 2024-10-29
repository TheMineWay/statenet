import { test, expect } from "@playwright/test";
import { BASE_URL, PROD_BASE_URL } from "../src/constants/base-url.constant";
import { parseStringPromise } from "xml2js";

function getHost(url: string): string {
  const parsedUrl = new URL(url);
  return parsedUrl.host;
}

function removeHost(url: string): string {
  const parsedUrl = new URL(url);
  return (
    getHost(BASE_URL) +
    parsedUrl.pathname +
    (parsedUrl.search || "") +
    (parsedUrl.hash || "")
  );
}

const parseXMLToUrls = async (xml: string): Promise<string[]> => {
  const result = await parseStringPromise(xml);
  const urls = result.urlset.url.map((urlObj: any) => urlObj.loc[0]);
  return urls;
};

const fetchSitemap = async () => {
  try {
    const text = await (await fetch(`${BASE_URL}/sitemap.xml`)).text();
    return text;
  } catch (e) {
    const text = await (await fetch(`${PROD_BASE_URL}/sitemap.xml`)).text();
    return text;
  }
};

const getRoutes = async () => {
  const parsed = await parseXMLToUrls(await fetchSitemap());
  return parsed.map(removeHost);
};

test("Proper page title", async ({ page }) => {
  for (const route of await getRoutes()) {
    await page.goto(route);

    expect(await page.title()).toContain("StateNet");
  }
});
