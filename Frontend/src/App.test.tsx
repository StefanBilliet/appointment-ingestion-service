import { Provider } from 'react-redux';
import { render, screen } from '@testing-library/react';
import { describe, expect, it } from 'vitest';
import App from './app';
import { createAppStore } from './state/store';

describe('App', () => {
  const renderApp = () =>
    render(
      <Provider store={createAppStore()}>
        <App />
      </Provider>,
    );

  it('renders the placeholder content', () => {
    renderApp();
    expect(
      screen.getByRole('heading', { level: 1, name: /appointments sandbox/i }),
    ).toBeInTheDocument();

    expect(screen.getByText(/storybook mock/i)).toBeInTheDocument();
    expect(screen.getByRole('alert')).toBeInTheDocument();
  });
});
