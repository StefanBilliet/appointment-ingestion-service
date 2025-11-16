import { setupServer, type SetupServerApi } from 'msw/node';
import { afterAll, afterEach, beforeAll } from 'vitest';

export const setupMswServer = (...handlers: Parameters<typeof setupServer>) => {
  const server: SetupServerApi = setupServer(...handlers);
  beforeAll(() => server.listen());
  afterEach(() => server.resetHandlers());
  afterAll(() => server.close());

  return server;
};
