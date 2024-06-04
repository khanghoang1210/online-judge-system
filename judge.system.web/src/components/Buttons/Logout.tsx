
import React from 'react';
import { Cookies, useCookies } from 'react-cookie';
import {FiLogOut} from 'react-icons/fi'


const Logout:React.FC = () => {
    const [cookies, setCookie, removeCookie] = useCookies(['token']);
    const handleLogout = () =>{
        removeCookie('token', { path: '/' });
        window.location.reload()
        
    }
    return <button className='bg-dark-fill-3 py-1.5 px-3 cursor-pointer rounded text-brand-orange' onClick={handleLogout}>
        <FiLogOut/>
    </button>
}
export default Logout;