import { useEffect, useState } from 'react';
import { Button, Card, Form, Stack } from 'react-bootstrap';

export type AppointmentFormValues = {
  clientName: string;
  appointmentTime: string;
  duration: number;
};

export type AppointmentFormProps = {
  initialClientName?: string;
  initialAppointmentTime?: string;
  initialDuration?: number;
  onSubmit?: (values: AppointmentFormValues) => void;
  onReset?: () => void;
};

const AppointmentForm = ({
  initialClientName = '',
  initialAppointmentTime = '',
  initialDuration = 30,
  onSubmit,
  onReset,
}: AppointmentFormProps) => {
  const [clientName, setClientName] = useState(initialClientName);
  const [appointmentTime, setAppointmentTime] = useState(initialAppointmentTime);
  const [duration, setDuration] = useState(initialDuration);

  useEffect(() => setClientName(initialClientName), [initialClientName]);
  useEffect(() => setAppointmentTime(initialAppointmentTime), [initialAppointmentTime]);
  useEffect(() => setDuration(initialDuration), [initialDuration]);

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    onSubmit?.({ clientName, appointmentTime, duration });
  };

  const handleReset = () => {
    setClientName(initialClientName);
    setAppointmentTime(initialAppointmentTime);
    setDuration(initialDuration);
    onReset?.();
  };

  return (
    <Card>
      <Card.Body>
        <Form onSubmit={handleSubmit}>
          <Stack gap={3}>
            <Form.Group controlId="clientName">
              <Form.Label>Client name</Form.Label>
              <Form.Control
                value={clientName}
                onChange={(event) => setClientName(event.target.value)}
                placeholder="Alice Johnson"
              />
            </Form.Group>
            <Form.Group controlId="appointmentTime">
              <Form.Label>Appointment time</Form.Label>
              <Form.Control
                type="datetime-local"
                value={appointmentTime}
                onChange={(event) => setAppointmentTime(event.target.value)}
              />
            </Form.Group>
            <Form.Group controlId="duration">
              <Form.Label>Duration (minutes)</Form.Label>
              <Form.Control
                type="number"
                min={15}
                step={15}
                value={duration}
                onChange={(event) => setDuration(Number(event.target.value))}
              />
            </Form.Group>
            <div className="d-flex gap-2">
              <Button type="submit">Mock submit</Button>
              <Button variant="outline-secondary" type="button" onClick={handleReset}>
                Reset
              </Button>
            </div>
          </Stack>
        </Form>
      </Card.Body>
    </Card>
  );
};

export default AppointmentForm;
