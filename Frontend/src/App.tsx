import { Container, Stack } from 'react-bootstrap';
import './App.css';
import AppointmentForm, { type AppointmentFormValues } from './components/AppointmentForm';

function App() {
  const handleSubmit = ({ clientName, appointmentTime, duration }: AppointmentFormValues) => {
    // Placeholder until backend wiring is implemented.
    // eslint-disable-next-line no-alert
    alert(
      `Mock submit:\nClient: ${clientName}\nTime: ${appointmentTime}\nDuration: ${duration} minutes`,
    );
  };

  return (
    <Container className="py-5">
      <Stack gap={4}>
        <div>
          <h1 className="mb-1">Appointment Ingestion</h1>
          <p className="text-muted mb-0">
            Lightweight React shell powered by Vite, React-Bootstrap, and Vitest.
          </p>
        </div>
        <AppointmentForm onSubmit={handleSubmit} />
      </Stack>
    </Container>
  );
}

export default App;
