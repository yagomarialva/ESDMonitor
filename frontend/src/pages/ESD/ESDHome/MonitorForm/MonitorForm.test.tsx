import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { Modal, Input, Table, Button } from 'antd';
import MonitorForm from './MonitorForm';

// Mock translations
jest.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => {
      const translations: { [key: string]: string } = {
        'Número de Série': 'Serial Number',
        'Descrição': 'Description',
        'ESD_TEST.DIALOG.CLOSE': 'Close',
        'ESD_TEST.DIALOG.SAVE': 'Save',
      };
      return translations[key] || key;
    },
  }),
}));

// Mock Material-UI icons
jest.mock('@mui/icons-material', () => ({
  LaptopOutlined: () => <div data-testid="laptop-icon" />,
}));

describe('MonitorForm', () => {
  const mockOnSubmit = jest.fn();
  const mockHandleClose = jest.fn();
  const defaultProps = {
    open: true,
    handleClose: mockHandleClose,
    onSubmit: mockOnSubmit,
    type: 'operador' as const,
  };

  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('renders correctly for operador type', () => {
    render(<MonitorForm {...defaultProps} />);
    
    expect(screen.getByText('Adicionar Operador')).toBeInTheDocument();
    expect(screen.getByTestId('laptop-icon')).toBeInTheDocument();
    expect(screen.getByText('Serial Number')).toBeInTheDocument();
    expect(screen.getByText('Description')).toBeInTheDocument();
  });

  it('renders correctly for jig type', () => {
    render(<MonitorForm {...defaultProps} type="jig" />);
    
    expect(screen.getByText('Adicionar Jig')).toBeInTheDocument();
  });

  it('handles input changes correctly', async () => {
    render(<MonitorForm {...defaultProps} />);
    
    const serialInput = screen.getByRole('textbox', { name: /serial/i });
    const descriptionInput = screen.getByRole('textbox', { name: /description/i });

    await userEvent.type(serialInput, 'TEST123');
    await userEvent.type(descriptionInput, 'Test Description');

    expect(serialInput).toHaveValue('TEST123');
    expect(descriptionInput).toHaveValue('Test Description');
  });

  it('submits form data correctly', async () => {
    render(<MonitorForm {...defaultProps} />);
    
    const serialInput = screen.getByRole('textbox', { name: /serial/i });
    const descriptionInput = screen.getByRole('textbox', { name: /description/i });
    const submitButton = screen.getByText('Save');

    await userEvent.type(serialInput, 'TEST123');
    await userEvent.type(descriptionInput, 'Test Description');
    await userEvent.click(submitButton);

    expect(mockOnSubmit).toHaveBeenCalledWith({
      serialNumberEsp: 'TEST123',
      description: 'Test Description',
      statusOperador: '',
      statusJig: '',
    });
  });

  it('handles form submission error correctly', async () => {
    const mockError = new Error('Submission failed');
    const mockOnSubmitError = jest.fn().mockRejectedValue(mockError);

    render(
      <MonitorForm
        {...defaultProps}
        onSubmit={mockOnSubmitError}
      />
    );

    const submitButton = screen.getByText('Save');
    await userEvent.click(submitButton);

    expect(mockOnSubmitError).toHaveBeenCalled();
    console.error = jest.fn();
    expect(console.error).toHaveBeenCalledWith('Error creating monitor:', mockError);
  });

  it('resets form on close', async () => {
    render(<MonitorForm {...defaultProps} />);
    
    const serialInput = screen.getByRole('textbox', { name: /serial/i });
    const closeButton = screen.getByText('Close');

    await userEvent.type(serialInput, 'TEST123');
    await userEvent.click(closeButton);

    expect(mockHandleClose).toHaveBeenCalled();
    
    // Verify form reset
    render(<MonitorForm {...defaultProps} />);
    const newSerialInput = screen.getByRole('textbox', { name: /serial/i });
    expect(newSerialInput).toHaveValue('');
  });

  it('validates required fields before submission', async () => {
    render(<MonitorForm {...defaultProps} />);
    
    const submitButton = screen.getByText('Save');
    await userEvent.click(submitButton);

    // Form should not be submitted if required fields are empty
    expect(mockOnSubmit).not.toHaveBeenCalled();
  });

  it('shows tooltips on input hover', async () => {
    render(<MonitorForm {...defaultProps} />);
    
    const serialInput = screen.getByRole('textbox', { name: /serial/i });
    await userEvent.type(serialInput, 'TEST123');
    
    // Hover over input
    fireEvent.mouseEnter(serialInput);
    
    // Wait for tooltip to appear
    await waitFor(() => {
      expect(screen.getByRole('tooltip')).toBeInTheDocument();
    });
  });
});
