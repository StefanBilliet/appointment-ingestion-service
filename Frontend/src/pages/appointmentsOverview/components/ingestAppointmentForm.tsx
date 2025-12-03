import {zodResolver} from '@hookform/resolvers/zod';
import {Button, Card, Form, Stack} from 'react-bootstrap';
import {useForm} from 'react-hook-form';
import {useIngestAppointmentMutation} from '../data/appointmentsApi.ts';
import {
  ingestAppointmentSchema,
  type IngestAppointmentFormInput,
  type IngestAppointmentFormValues,
} from '../validators/ingestAppointmentFormSchema.ts';

export const IngestAppointmentForm = () => {
  const [ingestAppointment, {isLoading}] = useIngestAppointmentMutation();
  const {
    register,
    handleSubmit,
    formState: {errors},
    reset,
  } = useForm<IngestAppointmentFormInput, undefined, IngestAppointmentFormValues>({
    resolver: zodResolver(ingestAppointmentSchema),
    defaultValues: {
      clientName: '',
      appointmentTime: '',
      serviceDuration: '',
    },
  });

  const onSubmit = handleSubmit(async (formValues) => {
    const payload = {
      ...formValues,
      appointmentTime: new Date(formValues.appointmentTime).toISOString(),
    };
    await ingestAppointment(payload).unwrap();
    reset();
  });

  return (
    <Card>
      <Card.Body>
        <Stack gap={3}>
          <Stack gap={1}>
            <Card.Title as="h2">Ingest a new appointment</Card.Title>
          </Stack>
          <Form noValidate onSubmit={onSubmit}>
            <Stack gap={3}>
              <Form.Group controlId="clientName">
                <Form.Label>Client name</Form.Label>
                <Form.Control
                  placeholder="e.g. Alice Johnson"
                  {...register('clientName')}
                  isInvalid={Boolean(errors.clientName)}
                />
                <Form.Control.Feedback type="invalid">
                  {errors.clientName?.message}
                </Form.Control.Feedback>
              </Form.Group>
              <Form.Group controlId="appointmentTime">
                <Form.Label>Appointment time</Form.Label>
                <Form.Control
                  type="datetime-local"
                  {...register('appointmentTime')}
                  isInvalid={Boolean(errors.appointmentTime)}
                />
                <Form.Control.Feedback type="invalid">
                  {errors.appointmentTime?.message}
                </Form.Control.Feedback>
              </Form.Group>
              <Form.Group controlId="serviceDuration">
                <Form.Label>Duration (minutes)</Form.Label>
                <Form.Control
                  type="text"
                  placeholder="30"
                  inputMode="numeric"
                  pattern="[0-9]*"
                  {...register('serviceDuration')}
                  isInvalid={Boolean(errors.serviceDuration)}
                />
                <Form.Control.Feedback type="invalid">
                  {errors.serviceDuration?.message}
                </Form.Control.Feedback>
              </Form.Group>
              <Stack direction="horizontal" gap={2}>
                <Button type="submit" disabled={isLoading}>
                  Submit
                </Button>
                <Button type="button" variant="outline-secondary" onClick={() => reset()}>
                  Reset
                </Button>
              </Stack>
            </Stack>
          </Form>
        </Stack>
      </Card.Body>
    </Card>
  );
};
