import {Container, Stack} from "react-bootstrap";
import {IngestAppointmentForm} from "./ingestAppointmentForm.tsx";
import AppointmentsList from "./appointments.tsx";

export const AppointmentsOverview = () => (
  <Container>
    <Stack gap={4}>
      <h1>Appointments overview</h1>
      <IngestAppointmentForm />
      <AppointmentsList />
    </Stack>
  </Container>
);