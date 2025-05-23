import {
  CurrentAccountsEndpointsClient,
  DevEndpointsClient,
  IngEndpointsClient,
  OperationsEndpointsClient,
} from "./apiClient.g.nswag";

class ApiClient {
  public readonly devEndpointsClient: DevEndpointsClient;
  public readonly operationsEndpointsClient: OperationsEndpointsClient;
  public readonly currentAccountEndpointsClient: CurrentAccountsEndpointsClient;
  public readonly ingEndpointsClient: IngEndpointsClient;

  constructor() {
    this.devEndpointsClient = new DevEndpointsClient("/api");
    this.operationsEndpointsClient = new OperationsEndpointsClient("/api");
    this.currentAccountEndpointsClient = new CurrentAccountsEndpointsClient("/api");
    this.ingEndpointsClient = new IngEndpointsClient("/api");
  }
}

const apiClient = new ApiClient();

export default apiClient;
