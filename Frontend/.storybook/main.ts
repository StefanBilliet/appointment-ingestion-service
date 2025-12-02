import '../src/test/setupLocalStorage.ts';
import type {StorybookConfig} from '@storybook/react-vite';
import {mergeConfig} from 'vite';

const config: StorybookConfig = {
  "stories": [
    "../src/**/*.mdx",
    "../src/**/*.stories.@(js|jsx|mjs|ts|tsx)"
  ],
  "addons": [
    "@chromatic-com/storybook",
    "@storybook/addon-docs",
    "@storybook/addon-onboarding",
    "@storybook/addon-a11y",
    "@storybook/addon-vitest",
    "msw-storybook-addon"
  ],
  "framework": {
    "name": "@storybook/react-vite",
    "options": {}
  },
  viteFinal: (baseConfig) =>
    mergeConfig(baseConfig, {
      build: {
        chunkSizeWarningLimit: 1500,
        rollupOptions: {
          output: {
            manualChunks: {
              react: ['react', 'react-dom'],
              redux: ['@reduxjs/toolkit', 'react-redux'],
              bootstrap: ['bootstrap', 'react-bootstrap'],
              storybook: [
                '@storybook/addon-a11y',
                '@storybook/addon-docs',
                '@storybook/addon-onboarding',
                '@storybook/addon-vitest',
                '@chromatic-com/storybook',
              ],
              msw: ['msw', 'msw-storybook-addon'],
              validation: ['zod'],
              axe: ['axe-core'],
            },
          },
        },
      },
    })
};
export default config;
