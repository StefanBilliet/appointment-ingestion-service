import {ensureMemoryLocalStorage} from '../src/test/setupLocalStorage.ts';
import type { Preview } from '@storybook/react-vite';
import { initialize, mswLoader } from 'msw-storybook-addon';
import 'bootstrap/dist/css/bootstrap.min.css';
import '@/index.css';
import '@/app.css';

// Storybook's Node build lacks browser localStorage; provide an in-memory fallback for msw.
ensureMemoryLocalStorage();

initialize();

const preview: Preview = {
  loaders: [mswLoader],
  parameters: {
    controls: {
      matchers: {
        color: /(background|color)$/i,
        date: /Date$/i,
      },
    },
    a11y: {
      test: 'todo',
    },
  },
};

export default preview;
