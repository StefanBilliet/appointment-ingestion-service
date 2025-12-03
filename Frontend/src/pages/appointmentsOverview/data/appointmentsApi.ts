import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type {IngestedAppointmentListItem} from "./ingestedAppointmentListItem.ts";
import type {AppointmentToBeIngested} from "./appointmentToBeIngested.ts";
import type {AppointmentIngestionConfirmation} from "./appointmentIngestionConfirmation.ts";

const resolveBaseUrl = () => {
  if (globalThis.window?.location?.origin) {
    return globalThis.window.location.origin.replace(/\/$/, '');
  }

  const envBase = (
    import.meta.env.VITE_API_BASE_URL
    ?? import.meta.env.API_HTTP
  )?.replace(/\/$/, '');
  return envBase ?? 'http://localhost';
};

export const appointmentsApi = createApi({
  reducerPath: 'appointmentsApi',
  baseQuery: fetchBaseQuery({ baseUrl: `${resolveBaseUrl()}/api/appointments` }),
  tagTypes: ['AppointmentList'],
  endpoints: (builder) => ({
    getAppointments: builder.query<IngestedAppointmentListItem[], void>({
      query: () => '',
      providesTags: ['AppointmentList'],
    }),
    ingestAppointment: builder.mutation<AppointmentIngestionConfirmation, AppointmentToBeIngested>({
      query: (payload) => ({
        url: '/ingest',
        method: 'POST',
        body: payload,
      }),
      invalidatesTags: ['AppointmentList'],
    }),
  }),
});

export const { useGetAppointmentsQuery, useIngestAppointmentMutation } = appointmentsApi;
