import {z} from 'zod';

const FIVE_MINUTES_MS = 5 * 60 * 1000;

const parseLocalDateTime = (value: string) => {
  const parsed = new Date(value);
  return Number.isNaN(parsed.getTime()) ? null : parsed;
};

const isAtLeastFiveMinutesAhead = (value: string) => {
  const date = parseLocalDateTime(value);
  if (!date) {
    return false;
  }

  return date.getTime() >= Date.now() + FIVE_MINUTES_MS;
};

const isOnHourOrHalfHour = (value: string) => {
  const date = parseLocalDateTime(value);
  if (!date) {
    return false;
  }

  const minutes = date.getMinutes();
  return minutes === 0 || minutes === 30;
};

export const ingestAppointmentSchema = z.object({
  clientName: z.string().trim().min(1, 'Client name is required'),
  appointmentTime: z
    .string()
    .trim()
    .min(1, 'Appointment time is required')
    .refine(isAtLeastFiveMinutesAhead, 'Appointment time must be at least 5 minutes in the future')
    .refine(isOnHourOrHalfHour, 'Appointment time must start on the hour or half-hour'),
  serviceDuration: z
    .string()
    .transform((value) => value.trim())
    .refine((value) => value === '' || !Number.isNaN(Number(value)), 'Duration must be a number')
    .transform((value) => (value === '' ? undefined : Number(value))),
});

export type IngestAppointmentFormInput = z.input<typeof ingestAppointmentSchema>;
export type IngestAppointmentFormValues = z.output<typeof ingestAppointmentSchema>;
