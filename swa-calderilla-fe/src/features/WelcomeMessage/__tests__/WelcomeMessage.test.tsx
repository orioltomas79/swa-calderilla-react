import { render, screen } from '@testing-library/react';
import { OtherEndpointsClient } from '../../../api/apiClient.g.nswag';
import WelcomeMessage from '..';


jest.mock('../../../api/apiClient.g.nswag', () => ({
  OtherEndpointsClient: jest.fn().mockImplementation(() => ({
    getMessage: jest.fn(),
  })),
}));

describe('WelcomeMessage Component', () => {
  let mockGetMessage = "dummy message";

  beforeEach(() => {
    mockGetMessage = jest.fn();
    OtherEndpointsClient.mockImplementation(() => ({
      getMessage: mockGetMessage,
    }));
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  test('renders the button and no initial message or error', () => {
    render(<WelcomeMessage />);

    expect(screen.getByText('Fetch API Data')).toBeInTheDocument();
  });
});
