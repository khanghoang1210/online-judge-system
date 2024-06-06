import AuthModal from '@/components/Modals/AuthModal';
import Navbar from '@/components/Navbar/Navbar';
import Image from 'next/image';
import React, { use, useEffect, useState } from 'react';
import { useRecoilValue } from 'recoil';
import {authModalState} from "@/atoms/authModalAtom";
import { useCookies } from 'react-cookie';
import { useRouter } from 'next/router';

type AuthPageProps = {
    
};

const AuthPage:React.FC<AuthPageProps> = () => {
    
    const authModal = useRecoilValue(authModalState)
    const [pageLoading, setPageLoading] = useState(true)
    const [cookies, setCookie] = useCookies(['token'])

    const router = useRouter()

    useEffect(() => {
        // Check if token exists
        if (cookies.token) {
            // User is logged in, redirect to the home page or dashboard
            router.push('/');
        } else {
            // User is not logged in
            setPageLoading(false);
        }
    }, [cookies, router]);

    if(pageLoading) return null;

    return <div className='bg-gradient-to-b from-gray-600 to-black h-screen relative'>
        <div className='max-w-7xl mx-auto'>
            <Navbar/>
            <div className='flex items-center justify-center h-[calc(100vh-5rem)] pointer-events-none select-none'>
					<Image src='/hero.png' alt='Hero img' width={500} height={500} />
				</div>
               {authModal.isOpen && <AuthModal/>} 
        </div>
    </div>
}
export default AuthPage;