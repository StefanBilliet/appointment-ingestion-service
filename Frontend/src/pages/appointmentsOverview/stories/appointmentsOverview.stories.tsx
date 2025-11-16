import type { Meta, StoryObj } from '@storybook/react';
import { Button, Card, Container, Form, ListGroup, Stack } from 'react-bootstrap';

const MOCK_APPOINTMENTS = [
  {
    id: 'apt-001',
    clientName: 'Alice Johnson',
    appointmentTime: '2025-02-01T10:30:00',
    duration: 45,
  },
  {
    id: 'apt-002',
    clientName: 'Ben Graham',
    appointmentTime: '2025-02-01T13:15:00',
    duration: 30,
  },
  {
    id: 'apt-003',
    clientName: 'Priya Patel',
    appointmentTime: '2025-02-02T09:00:00',
    duration: 60,
  },
];

const MockAppointmentsOverview = () => (
  <Container>
    <Stack gap={4}>
      <Stack gap={1}>
        <h1>Appointments overview</h1>
        <Form.Text muted>
          Mock layout showing the ingest form and the resulting appointment list.
        </Form.Text>
      </Stack>
      <Card>
        <Card.Body>
          <Stack gap={3}>
            <Stack gap={1}>
              <Card.Title as="h2">Ingest a new appointment</Card.Title>
              <Form.Text muted>
                Static mockup — fields are not connected to any data source.
              </Form.Text>
            </Stack>
            <Form>
              <Stack gap={3}>
                <Form.Group controlId="mockClientName">
                  <Form.Label>Client name</Form.Label>
                  <Form.Control placeholder="e.g. Alice Johnson" />
                </Form.Group>
                <Form.Group controlId="mockAppointmentTime">
                  <Form.Label>Appointment time</Form.Label>
                  <Form.Control type="datetime-local" />
                </Form.Group>
                <Form.Group controlId="mockDuration">
                  <Form.Label>Duration (minutes)</Form.Label>
                  <Form.Control type="number" min={15} step={15} placeholder="30" />
                </Form.Group>
                <Stack direction="horizontal" gap={2}>
                  <Button type="button">Submit (mock)</Button>
                  <Button type="button" variant="outline-secondary">
                    Reset
                  </Button>
                </Stack>
              </Stack>
            </Form>
          </Stack>
        </Card.Body>
      </Card>
      <Card>
        <Card.Body>
          <Stack gap={3}>
            <Stack direction="horizontal" gap={3}>
              <Stack gap={1}>
                <Card.Title as="h2">Recently ingested</Card.Title>
                <Form.Text muted>Static sample data for design reference.</Form.Text>
              </Stack>
              <Form.Text muted>{MOCK_APPOINTMENTS.length} appointments</Form.Text>
            </Stack>
          </Stack>
          <ListGroup variant="flush">
            {MOCK_APPOINTMENTS.map((appointment) => (
              <ListGroup.Item key={appointment.id}>
                <Stack direction="horizontal" gap={3}>
                  <Stack gap={0}>
                    <strong>{appointment.clientName}</strong>
                    <Form.Text muted>
                      {new Date(appointment.appointmentTime).toLocaleString()} •{' '}
                      {appointment.duration} minutes
                    </Form.Text>
                  </Stack>
                  <Form.Text muted>ID: {appointment.id}</Form.Text>
                </Stack>
              </ListGroup.Item>
            ))}
          </ListGroup>
        </Card.Body>
      </Card>
    </Stack>
  </Container>
);

const meta = {
  title: 'Pages/AppointmentsOverview/Mock',
  component: MockAppointmentsOverview,
  parameters: {
    controls: { hideNoControlsWarning: true },
  },
} satisfies Meta<typeof MockAppointmentsOverview>;

export default meta;

type Story = StoryObj<typeof meta>;

export const OverviewMock: Story = {};
