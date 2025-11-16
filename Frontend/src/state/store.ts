import { configureStore } from '@reduxjs/toolkit';
import { appointmentsApi } from './appointmentsApi';

export const createAppStore = () =>
  configureStore({
    reducer: {
      [appointmentsApi.reducerPath]: appointmentsApi.reducer,
    },
    middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(appointmentsApi.middleware),
  });

export const store = createAppStore();
export type AppStore = ReturnType<typeof createAppStore>;
export type RootState = ReturnType<AppStore['getState']>;
export type AppDispatch = AppStore['dispatch'];
