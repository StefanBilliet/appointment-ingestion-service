import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { Appointment, CreateAppointmentRequest } from '../../../api/appointments.ts';

const resolveBaseUrl = () => {
  if (typeof window !== 'undefined' && window.location?.origin) {
    return window.location.origin.replace(/\/$/, '');
  }

  const envBase = (import.meta.env.VITE_API_BASE_URL as string | undefined)?.replace(/\/$/, '');
  return envBase ?? 'http://localhost';
};

export const appointmentsApi = createApi({
  reducerPath: 'appointmentsApi',
  baseQuery: fetchBaseQuery({ baseUrl: `${resolveBaseUrl()}/api/appointments` }),
  tagTypes: ['AppointmentList'],
  endpoints: (builder) => ({
    getAppointments: builder.query<Appointment[], void>({
      query: () => '',
      providesTags: ['AppointmentList'],
    }),
    ingestAppointment: builder.mutation<Appointment, CreateAppointmentRequest>({
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
