import {render, screen, waitFor} from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import {http, HttpResponse} from 'msw';
import {Provider} from 'react-redux';
import {afterEach, describe, expect, test, vi} from 'vitest';
import type {AppointmentToBeIngested} from '../../../api/appointmentToBeIngested.ts';
import {createAppStore} from '../../../state/store';
import {setupMswServer} from '../../../test/setupMsw';
import {IngestAppointmentForm} from '../components/ingestAppointmentForm';

const server = setupMswServer();
afterEach(() => {
  vi.restoreAllMocks();
});

describe('IngestAppointmentForm', () => {
  test('GIVEN empty form WHEN submit immediately THEN show validation errors and do not ingest', async () => {
    const ingestSpy = vi.fn();
    server.use(
      http.post('*/api/appointments/ingest', async () => {
        ingestSpy();
        return HttpResponse.json({id: 'apt-123'});
      }),
    );
    render(
      <Provider store={createAppStore()}>
        <IngestAppointmentForm/>
      </Provider>,
    );

    const user = userEvent.setup();
    await user.click(screen.getByRole('button', {name: /submit/i}));

    await screen.findByText(/Client name is required/i);
    await screen.findByText(/Appointment time is required/i);
    await waitFor(() => expect(ingestSpy).not.toHaveBeenCalled());
  });

  test('GIVEN filled form WHEN submit THEN call ingest endpoint via mutation', async () => {
    vi.spyOn(Date, 'now').mockReturnValue(new Date('2025-01-01T09:00:00').getTime());
    const ingestSpy = vi.fn();
    server.use(
      http.post('*/api/appointments/ingest', async ({request}) => {
        const payload = (await request.json()) as AppointmentToBeIngested;
        ingestSpy(payload);
        return HttpResponse.json({id: 'apt-999'});
      }),
    );
    render(
      <Provider store={createAppStore()}>
        <IngestAppointmentForm/>
      </Provider>,
    );

    const user = userEvent.setup();
    await user.type(screen.getByLabelText(/client name/i), 'Alice Johnson');
    await user.type(screen.getByLabelText(/appointment time/i), '2025-02-01T10:30');
    await user.type(screen.getByLabelText(/duration/i), '45');
    await user.click(screen.getByRole('button', {name: /submit/i}));

    await waitFor(() =>
      expect(ingestSpy).toHaveBeenCalledWith({
        clientName: 'Alice Johnson',
        appointmentTime: '2025-02-01T10:30',
        duration: 45,
      }),
    );
  });

  test('GIVEN duration omitted WHEN submit THEN ingest with remaining fields', async () => {
    vi.spyOn(Date, 'now').mockReturnValue(new Date('2025-01-01T09:00:00').getTime());
    const ingestSpy = vi.fn();
    server.use(
      http.post('*/api/appointments/ingest', async ({request}) => {
        const payload = (await request.json()) as AppointmentToBeIngested;
        ingestSpy(payload);
        return HttpResponse.json({id: 'apt-1000'});
      }),
    );
    render(
      <Provider store={createAppStore()}>
        <IngestAppointmentForm/>
      </Provider>,
    );

    const user = userEvent.setup();
    await user.type(screen.getByLabelText(/client name/i), 'Ben Graham');
    await user.type(screen.getByLabelText(/appointment time/i), '2025-02-01T13:30');
    await user.click(screen.getByRole('button', {name: /submit/i}));

    await waitFor(() =>
      expect(ingestSpy).toHaveBeenCalledWith({
        clientName: 'Ben Graham',
        appointmentTime: '2025-02-01T13:30',
      }),
    );
  });

  test('GIVEN invalid duration WHEN submit THEN show duration error and skip ingest', async () => {
    vi.spyOn(Date, 'now').mockReturnValue(new Date('2025-01-01T09:00:00').getTime());
    const ingestSpy = vi.fn();
    server.use(
      http.post('*/api/appointments/ingest', async ({request}) => {
        const payload = (await request.json()) as AppointmentToBeIngested;
        ingestSpy(payload);
        return HttpResponse.json({id: 'apt-1500'});
      }),
    );
    render(
      <Provider store={createAppStore()}>
        <IngestAppointmentForm/>
      </Provider>,
    );

    const user = userEvent.setup();
    await user.type(screen.getByLabelText(/client name/i), 'Casey Freeform');
    await user.type(screen.getByLabelText(/appointment time/i), '2025-02-01T14:30');
    await user.type(screen.getByLabelText(/duration/i), 'e');
    await user.click(screen.getByRole('button', {name: /submit/i}));

    await screen.findByText(/duration must be a number/i);
    await waitFor(() => expect(ingestSpy).not.toHaveBeenCalled());
  });

  test('GIVEN appointment time less than five minutes away WHEN submit THEN show time error', async () => {
    vi.spyOn(Date, 'now').mockReturnValue(new Date('2025-02-01T10:00:00').getTime());
    const ingestSpy = vi.fn();
    server.use(
      http.post('*/api/appointments/ingest', async ({request}) => {
        const payload = (await request.json()) as AppointmentToBeIngested;
        ingestSpy(payload);
        return HttpResponse.json({id: 'apt-2000'});
      }),
    );

    render(
      <Provider store={createAppStore()}>
        <IngestAppointmentForm/>
      </Provider>,
    );

    const user = userEvent.setup();
    await user.type(screen.getByLabelText(/client name/i), 'Near Future');
    await user.type(screen.getByLabelText(/appointment time/i), '2025-02-01T10:04');
    await user.click(screen.getByRole('button', {name: /submit/i}));

    await screen.findByText(/appointment time must be at least 5 minutes in the future/i);
    await waitFor(() => expect(ingestSpy).not.toHaveBeenCalled());
  });

  test('GIVEN appointment time not on hour or half WHEN submit THEN show boundary error', async () => {
    vi.spyOn(Date, 'now').mockReturnValue(new Date('2025-02-01T10:00:00').getTime());
    const ingestSpy = vi.fn();
    server.use(
      http.post('*/api/appointments/ingest', async ({request}) => {
        const payload = (await request.json()) as AppointmentToBeIngested;
        ingestSpy(payload);
        return HttpResponse.json({id: 'apt-2001'});
      }),
    );

    render(
      <Provider store={createAppStore()}>
        <IngestAppointmentForm/>
      </Provider>,
    );

    const user = userEvent.setup();
    await user.type(screen.getByLabelText(/client name/i), 'Off Slot');
    await user.type(screen.getByLabelText(/appointment time/i), '2025-02-01T10:10');
    await user.click(screen.getByRole('button', {name: /submit/i}));

    await screen.findByText(/appointment time must start on the hour or half-hour/i);
    await waitFor(() => expect(ingestSpy).not.toHaveBeenCalled());
  });
});
