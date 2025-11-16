export type Appointment = {
  id: string;
  clientName: string;
  appointmentTime: string;
  duration: number;
};

export type CreateAppointmentRequest = {
  clientName: string;
  appointmentTime: string;
  duration: number;
};
