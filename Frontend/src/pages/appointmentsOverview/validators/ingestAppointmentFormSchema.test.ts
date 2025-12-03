import {afterEach, describe, expect, test, vi} from 'vitest';
import {ingestAppointmentSchema} from './ingestAppointmentFormSchema.ts';

const collectFieldErrors = (
  issues: Array<{path: (string | number | symbol)[]; message: string}>,
  field: string,
) => issues.filter((issue) => issue.path[0] === field).map((issue) => issue.message);

describe('ingestAppointmentFormSchema', () => {
  afterEach(() => {
    vi.restoreAllMocks();
  });

  test('accepts valid payload with optional duration', () => {
    vi.spyOn(Date, 'now').mockReturnValue(new Date('2025-02-01T09:00:00').getTime());

    const result = ingestAppointmentSchema.parse({
      clientName: 'Valid Person',
      appointmentTime: '2025-02-01T10:30',
      serviceDuration: '',
    });

    expect(result).toEqual({
      clientName: 'Valid Person',
      appointmentTime: '2025-02-01T10:30',
      serviceDuration: undefined,
    });
  });

  test('rejects appointment time less than five minutes ahead', () => {
    vi.spyOn(Date, 'now').mockReturnValue(new Date('2025-02-01T10:00:00').getTime());

    const result = ingestAppointmentSchema.safeParse({
      clientName: 'Too Soon',
      appointmentTime: '2025-02-01T10:03',
      serviceDuration: '',
    });

    expect(result.success).toBe(false);
    if (!result.success) {
      expect(collectFieldErrors(result.error.issues, 'appointmentTime')).toContain(
        'Appointment time must be at least 5 minutes in the future',
      );
    }
  });

  test('rejects appointment times not on the hour or half-hour', () => {
    vi.spyOn(Date, 'now').mockReturnValue(new Date('2025-02-01T10:00:00').getTime());

    const result = ingestAppointmentSchema.safeParse({
      clientName: 'Off Slot',
      appointmentTime: '2025-02-01T10:10',
      serviceDuration: '',
    });

    expect(result.success).toBe(false);
    if (!result.success) {
      expect(collectFieldErrors(result.error.issues, 'appointmentTime')).toContain(
        'Appointment time must start on the hour or half-hour',
      );
    }
  });
});
