import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";

const AppRoutes = [
  {
    path: '/',
    component: Home
  },
  {
    path: '/counter',
    component: Counter
  },
  {
    path: '/fetch-data',
    component: FetchData
  }
];

export default AppRoutes;
