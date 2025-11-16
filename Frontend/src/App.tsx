import { Alert, Card, Container, Stack } from 'react-bootstrap';
import './app.css';

const App = () => (
  <Container>
    <Stack gap={3}>
      <Card body>
        <Stack gap={2}>
          <Card.Title as="h1">Appointments Sandbox</Card.Title>
          <Card.Text>
            This is a temporary placeholder while the real appointments overview is being rebuilt.
          </Card.Text>
          <Card.Text>
            Peek at the Storybook mock (<code>Pages/AppointmentsOverview/Mock</code>) to see the
            planned ingest form and appointments list.
          </Card.Text>
        </Stack>
      </Card>
      <Alert variant="info">
        Implementation work will resume as soon as the backend contract is finalized.
      </Alert>
    </Stack>
  </Container>
);

export default App;
