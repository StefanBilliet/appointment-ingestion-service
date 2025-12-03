import type {Meta, StoryObj} from '@storybook/react';
import {http, HttpResponse} from 'msw';
import {Provider} from 'react-redux';
import {createAppStore} from '../../../state/store';
import type {IngestedAppointmentListItem} from "../data/ingestedAppointmentListItem.ts";
import type {AppointmentToBeIngested} from "../data/appointmentToBeIngested.ts";
import {AppointmentsOverview} from "../components/appointmentsOverview.tsx";

const MOCK_APPOINTMENTS:IngestedAppointmentListItem[] = [
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
  {
    id: 'apt-003',
    clientName: 'Priya Patel',
    appointmentTime: '2025-02-02T09:00:00',
    serviceDuration: 60,
  },
];



const meta = {
  title: 'Pages/AppointmentsOverview/Mock',
  component: AppointmentsOverview,
  parameters: {
    controls: {hideNoControlsWarning: true},
    msw: {
      handlers: (() => {
        let nextId = MOCK_APPOINTMENTS.length + 1;
        let appointments = [...MOCK_APPOINTMENTS];

        return [
          http.get('*/api/appointments', () => HttpResponse.json(appointments)),
          http.post('*/api/appointments/ingest', async ({request}) => {
            const payload = (await request.json()) as AppointmentToBeIngested;
            const newAppointment:IngestedAppointmentListItem = {
              id: `apt-${String(nextId).padStart(3, '0')}`,
              serviceDuration: 30,
              ...payload,
            };
            nextId += 1;
            appointments = appointments.concat(newAppointment);
            return HttpResponse.json(newAppointment);
          }),
        ];
      })(),
    },
  },
  decorators: [
    (Story) => (
      <Provider store={createAppStore()}>
        <Story />
      </Provider>
    ),
  ],
} satisfies Meta<typeof AppointmentsOverview>;

export default meta;

type Story = StoryObj<typeof meta>;

export const OverviewMock: Story = {};
