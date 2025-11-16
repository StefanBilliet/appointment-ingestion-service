import {zodResolver} from '@hookform/resolvers/zod';
import {Button, Card, Form, Stack} from 'react-bootstrap';
import {useForm} from 'react-hook-form';
import {z} from 'zod';
import {useIngestAppointmentMutation} from '../data/appointmentsApi.ts';

const ingestAppointmentSchema = z.object({
  clientName: z.string().trim().min(1, 'Client name is required'),
  appointmentTime: z.string().trim().min(1, 'Appointment time is required'),
  duration: z
    .string()
    .transform((value) => value.trim())
    .refine(
      (value) => value === '' || !Number.isNaN(Number(value)),
      'Duration must be a number',
    )
    .transform((value) => (value === '' ? undefined : Number(value))),
});

type IngestAppointmentFormInput = z.input<typeof ingestAppointmentSchema>;
type IngestAppointmentFormValues = z.output<typeof ingestAppointmentSchema>;

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
      duration: '',
    },
  });

  const onSubmit = handleSubmit(async (formValues) => {
    await ingestAppointment(formValues).unwrap();
    reset();
  });

  return (
    <Card>
      <Card.Body>
        <Stack gap={3}>
          <Stack gap={1}>
            <Card.Title as="h2">Ingest a new appointment</Card.Title>
            <Form.Text muted>
              Static mockup — fields are not connected to any data source.
            </Form.Text>
          </Stack>
          <Form noValidate onSubmit={onSubmit}>
            <Stack gap={3}>
              <Form.Group controlId="mockClientName">
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
              <Form.Group controlId="mockAppointmentTime">
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
              <Form.Group controlId="mockDuration">
                <Form.Label>Duration (minutes)</Form.Label>
                <Form.Control
                  type="text"
                  placeholder="30"
                  inputMode="numeric"
                  pattern="[0-9]*"
                  {...register('duration')}
                  isInvalid={Boolean(errors.duration)}
                />
                <Form.Control.Feedback type="invalid">
                  {errors.duration?.message}
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
