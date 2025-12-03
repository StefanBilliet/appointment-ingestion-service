import {Alert, Card, Form, ListGroup, Spinner, Stack} from 'react-bootstrap';
import {useGetAppointmentsQuery} from '../data/appointmentsApi.ts';

const AppointmentsList = () => {
  const { data: appointments = [], isFetching, isError } = useGetAppointmentsQuery();
  const title = 'Overview';
  const headingId = `${title.replaceAll(/\s+/g, '-').toLowerCase()}-heading`;

  if (isFetching) {
    return (
      <Card as="section" aria-labelledby={headingId}>
        <Card.Body>
          <Stack className="align-items-center">
            <output aria-live="polite">
              <Spinner animation="border">
                <span className="visually-hidden">Loading appointments…</span>
              </Spinner>
            </output>
          </Stack>
        </Card.Body>
      </Card>
    );
  }

  if (isError) {
    return (
      <Card as="section" aria-labelledby={headingId}>
        <Card.Body>
          <Alert variant="danger" role="alert">
            Unable to load appointments. Please try again later.
          </Alert>
        </Card.Body>
      </Card>
    );
  }

  return (
    <Card as="section" aria-labelledby={headingId}>
      <Card.Body>
        <Stack gap={3}>
          <Stack gap={1}>
            <Card.Title as="h2" id={headingId}>
              {title}
            </Card.Title>
          </Stack>
          {appointments.length > 0 ? (
            <ListGroup variant="flush" as="ul">
              {appointments.map((appointment) => (
                <ListGroup.Item as="li" key={appointment.id}>
                  <Stack gap={1}>
                    <strong>{appointment.clientName}</strong>
                    <Form.Text muted>
                      {new Date(appointment.appointmentTime).toLocaleString()}
                      {appointment.serviceDuration ? ` • ${appointment.serviceDuration} minutes` : null}
                    </Form.Text>
                  </Stack>
                </ListGroup.Item>
              ))}
            </ListGroup>
          ) : (
            <Alert variant="secondary" role="alert">
              No appointments yet. New entries will appear here after ingestion.
            </Alert>
          )}
        </Stack>
      </Card.Body>
    </Card>
  );
};

export default AppointmentsList;
