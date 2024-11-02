import Tgle from "@theme/ColorModeToggle";
import styles from "./index.module.css";
import { ColorMode, useColorMode } from "@docusaurus/theme-common";
import { ColorModeProvider } from "@docusaurus/theme-common/internal";
import useDocusaurusContext from "@docusaurus/useDocusaurusContext";
import clsx from "clsx";
import pk from "../../package.json";

export default function Index() {
  return (
    <ColorModeProvider>
      <Content />
    </ColorModeProvider>
  );
}

const Content = () => {
  const mode = useColorMode();
  const config = useDocusaurusContext();
  return (
    <div className={styles.content}>
      <div />
      <div>
        <h1 className={styles.title}>
          State<span className={styles.net}>.NET</span>
        </h1>
        <p>{config.siteConfig.tagline}</p>
        <div className={clsx(styles.centered, styles.actions)}>
          <a
            className="button button--outline button--secondary"
            href="docs/intro"
          >
            Docs
          </a>
          <a className="button button--outline button--secondary" href="blog">
            Blog
          </a>
          <a
            className="button button--outline button--secondary"
            href={pk.repository.url}
            target="_blank"
          >
            GitHub
          </a>
          <a
            className="button button--outline button--secondary"
            href="https://www.nuget.org/packages/TheMineWay.StateNet"
            target="_blank"
          >
            NuGet
          </a>
        </div>
      </div>
      <div className={styles.centered}>
        <Tgle
          value={mode.colorMode}
          onChange={function (colorMode: ColorMode): void {
            mode.setColorMode(colorMode);
          }}
        />
      </div>
    </div>
  );
};
