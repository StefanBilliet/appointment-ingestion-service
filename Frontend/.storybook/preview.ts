import type { Preview } from '@storybook/react-vite';
import { initialize, mswDecorator } from 'msw-storybook-addon';
import 'bootstrap/dist/css/bootstrap.min.css';
import '../src/index.css';
import '../src/app.css';

initialize();

const preview: Preview = {
  decorators: [mswDecorator],
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
