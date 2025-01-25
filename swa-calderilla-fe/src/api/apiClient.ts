import { OtherEndpointsClient } from "./apiClient.g.nswag";

class ApiClient {
  public readonly otherEndpoints: OtherEndpointsClient;

  constructor() {
    this.otherEndpoints = new OtherEndpointsClient("/api");
  }
}

const apiClient = new ApiClient();

export default apiClient;
