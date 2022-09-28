import { Route, Routes } from 'react-router-dom'
import App from '../App';
import { Auth } from '../pages/auth/Auth'
import { EnterPhone } from '../pages/auth/EnterPhone'
import { VerifyCode } from '../pages/auth/VerifyCode'
import Constructor from '../pages/Constructor';

export function AppRoutes() {
  return (
        <>
          <Routes>
              <Route path="/" element={<Constructor />} />
              <Route path="/auth" element={<Auth />}>
                  <Route index element={<EnterPhone />} />
                  <Route path="phone" element={<EnterPhone />} />
                  <Route path="code" element={<VerifyCode />} />
              </Route>
              <Route path="*" element={<h2>Ресурс не найден</h2>} />
          </Routes>
      </>
  );
}
