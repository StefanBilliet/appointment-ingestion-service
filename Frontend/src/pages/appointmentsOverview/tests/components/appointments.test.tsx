import {render, screen, within} from '@testing-library/react';
import {http, HttpResponse} from 'msw';
import {Provider} from 'react-redux';
import {describe, expect, test} from 'vitest';
import AppointmentsList from '../../components/appointments.tsx';
import {createAppStore} from '@/state/store.ts';
import {setupMswServer} from '@/test/setupMsw.ts';

const server = setupMswServer();

describe('AppointmentsList', () => {
  test('GIVEN no appointments WHEN render THEN show alert', async () => {
    server.use(http.get('*/api/appointments', () => HttpResponse.json([])));
    const store = createAppStore();

    render(
      <Provider store={store}>
        <AppointmentsList/>
      </Provider>,
    );

    const alert = await screen.findByRole('alert');
    expect(alert).toHaveTextContent(/no appointments yet/i);
  });

  test('GIVEN error when fetching appointments WHEN render THEN show alert', async () => {
    server.use(
      http.get('*/api/appointments', () =>
        HttpResponse.json({message: 'Server unavailable'}, {status: 500}),
      ),
    );
    const store = createAppStore();

    render(
      <Provider store={store}>
        <AppointmentsList/>
      </Provider>,
    );

    const alert = await screen.findByRole('alert');
    expect(alert).toHaveTextContent(/unable to load appointments/i);
  });

  test('GIVEN appointments WHEN render THEN show rows', async () => {
    const mockAppointments = [
      {
        id: 'apt-001',
        clientName: 'Alice Johnson',
        appointmentTime: '2025-02-01T10:30:00',
        serviceDuration: 45,
      },
      {
        id: 'apt-002',
        clientName: 'Ben Graham',
        appointmentTime: '2025-02-01T13:15:00',
        serviceDuration: 30,
      },
    ];
    server.use(http.get('*/api/appointments', () => HttpResponse.json(mockAppointments)));
    const store = createAppStore();

    render(
      <Provider store={store}>
        <AppointmentsList/>
      </Provider>,
    );

    const list = await screen.findByRole('list');
    const items = within(list).getAllByRole('listitem');
    expect(items).toHaveLength(2);
    expect(items[0]).toHaveTextContent(`${mockAppointments[0].clientName}` +
      `${new Date(mockAppointments[0].appointmentTime).toLocaleString()} • ${mockAppointments[0].serviceDuration} minutes`);
    expect(items[1]).toHaveTextContent(`${mockAppointments[1].clientName}` +
      `${new Date(mockAppointments[1].appointmentTime).toLocaleString()} • ${mockAppointments[1].serviceDuration} minutes`);
  });
});
