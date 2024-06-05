import AuthModal from '@/components/Modals/AuthModal';
import Navbar from '@/components/Navbar/Navbar';
import Image from 'next/image';
import React, { use, useEffect, useState } from 'react';
import { useRecoilValue } from 'recoil';
import {authModalState} from "@/atoms/authModalAtom";
import { useCookies } from 'react-cookie';
import { useRouter } from 'next/router';
import ResetPassword from '@/components/Modals/ResetPassword';
import { IoClose } from 'react-icons/io5';
import ForgotPassword from '@/components/Modals/ForgotPassword';

type RecoverPageProps = {
    
};

const RecoverPage:React.FC<RecoverPageProps> = () => {
    
    const authModal = useRecoilValue(authModalState)
    const [pageLoading, setPageLoading] = useState(true)
    const [cookies, setCookie] = useCookies(['token'])
   // const closeModal = useCloseModal();

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

    return (
            <div className='bg-gradient-to-b from-gray-600 to-black h-screen relative'>
            {/* <div className='absolute top-0 left-0 w-full h-full flex items-center justify-center 
                            bg-black bg-opacity-60'></div> */}
                              <Navbar/>
                              
            <div className='w-full sm:w-[450px]  absolute top-[50%] left-[50%] translate-x-[-50%] translate-y-[-50%]  flex justify-center items-center'>
                <div className='relative w-full h-full mx-auto flex items-center justify-center'>
                    <div className='bg-white rounded-lg shadow relative w-full bg-gradient-to-b from-brand-orange to-slate-900 mx-6'>
                        <div className='flex justify-end p-2'>
                    </div>
                    
                    <ForgotPassword/>
                </div>
            </div>
        </div>
        </div>
    )}
export default RecoverPage;