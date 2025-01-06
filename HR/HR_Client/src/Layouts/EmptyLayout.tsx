import { Outlet } from 'react-router-dom'

function EmptyLayout(){
    return (
        <main>
          <Outlet />
        </main>
    );
  };

  export default EmptyLayout;

