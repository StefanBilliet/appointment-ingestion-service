import { Card, Container, Stack } from 'react-bootstrap';
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
        </Stack>
      </Card>
    </Stack>
  </Container>
);

export default App;
