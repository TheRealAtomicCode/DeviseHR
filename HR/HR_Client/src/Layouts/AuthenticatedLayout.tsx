import { Outlet } from 'react-router-dom'
import SideNav from '../Components/Nav/SideNav' 

function AuthenticatedLayout(){
    return (
      <div className='ml-16'>
        <SideNav /> {/* This is the navigation bar */}
        <main>
          <Outlet /> {/* Nested route content will be rendered here */}
        </main>
      </div>
    );
  };

  export default AuthenticatedLayout;

