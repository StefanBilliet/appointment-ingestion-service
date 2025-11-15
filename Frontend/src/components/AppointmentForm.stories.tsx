import type { Meta, StoryObj } from '@storybook/react';
import AppointmentForm from './AppointmentForm';

const meta = {
  title: 'Appointment/AppointmentForm',
  component: AppointmentForm,
  argTypes: {
    onSubmit: { action: 'submit' },
    onReset: { action: 'reset' },
  },
} satisfies Meta<typeof AppointmentForm>;

export default meta;
type Story = StoryObj<typeof meta>;

export const EmptyForm: Story = {};

export const PrefilledValues: Story = {
  args: {
    initialClientName: 'Alice Johnson',
    initialAppointmentTime: '2024-12-01T10:30',
    initialDuration: 45,
  },
};
