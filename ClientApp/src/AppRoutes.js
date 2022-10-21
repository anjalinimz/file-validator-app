import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { FileValidator } from "./components/FileValidator";

const AppRoutes = [
  {
    index: true,
    element: <FileValidator />
  },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
