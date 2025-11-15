import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import App from './App';
import {describe, expect, it } from "vitest";

describe('App', () => {
  it('renders the ingestion form skeleton', async () => {
    render(<App />);
    expect(screen.getByRole('heading', { level: 1, name: /appointment ingestion/i })).toBeInTheDocument();

    const nameInput = screen.getByLabelText(/client name/i);
    await userEvent.type(nameInput, 'Jane Doe');
    expect(nameInput).toHaveValue('Jane Doe');
  });
});
