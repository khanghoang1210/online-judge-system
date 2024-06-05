import { authModalState } from '@/atoms/authModalAtom';
import { Cookie } from 'next/font/google';
import { useRouter } from 'next/router';
import React, { useEffect, useState } from 'react';
import { useSetRecoilState } from 'recoil';
import { CookiesProvider, useCookies } from 'react-cookie'
import 'react-toastify/dist/ReactToastify.css'
import { toast } from 'react-toastify';
import { useSearchParams } from 'react-router-dom';

type ForgotPasswordProps = {
    
};
const API_URL = 'http://localhost:5107/api/Accounts/ResetPassword';

const ForgotPassword:React.FC<ForgotPasswordProps> = () => {
    const [inputs, setInputs] = useState({password:"", confirmPassword:""});
    const router = useRouter();
   
    const handleChangeInput = (e:React.ChangeEvent<HTMLInputElement>)=>{
        setInputs((prev)=>({...prev,[e.target.name]:e.target.value}));

    }
    const [queryParam, setQueryParam] = useState('');

            useEffect(() => {
              // Lấy tham số trên URL hiện tại
              const searchParams = new URLSearchParams(window.location.search);
              const paramValue = searchParams.get('id');
              setQueryParam(paramValue || 'default'); // Nếu không có tham số, sử dụng giá trị mặc định
            }, []);

    const handleForgot = async(e:React.FormEvent<HTMLFormElement>)=>{
        e.preventDefault();
        if(!inputs.password || !inputs.confirmPassword) return alert("Please fill all field");
        try {
           
            const data = {password: inputs.password, confirmPassword: inputs.confirmPassword, token:queryParam}
            const res = await fetch(API_URL, {
                method: "POST",
                body: JSON.stringify(data),
                mode: "cors",
               
                headers: {
                    'Accept': 'application/json, text/plain',
                    'Content-Type': 'application/json;charset=UTF-8'
                },
              });
            if(!res.ok) {
                toast.error(res.statusText, { position: "top-center", autoClose: 3000, theme: "dark" });
            }
            const result = await res.json();
 
            if(result.statusCode == 400) {
                toast.warn("Unauthorize",{ position: "top-center", autoClose: 3000, theme: "dark" })

            }else if(result.statusCode == 200){
                toast.success("Reset password successful",
                { position: "top-center", autoClose: 4000, theme: "dark" })
                router.push("/auth")
            }
            
        } catch (error:any) {
            toast.error(error.message,{ position: "top-center", autoClose: 3000, theme: "dark" })
        }
    }
    return <form className='space-y-6 px-6 pb-4' onSubmit={handleForgot}>
    <h3 className='text-xl font-medium text-white'>Reset Password</h3>
    <div>
        <label htmlFor='password' className='text-sm font-medium block mb-2 text-gray-300'>
            New Password
        </label>
        <input
        onChange={handleChangeInput}
            type='password'
            name='password'
            id='password'
            className='
    border-2 outline-none sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5
    bg-gray-600 border-gray-500 placeholder-gray-400 text-white
'
            placeholder='*******'
        />
    </div>
    <div>
        <label htmlFor='password' className='text-sm font-medium block mb-2 text-gray-300'>
            Confirm Password
        </label>
        <input
        onChange={handleChangeInput}
            type='password'
            name='confirmPassword'
            id='confirmPassword'
            className='
    border-2 outline-none sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5
    bg-gray-600 border-gray-500 placeholder-gray-400 text-white
'
            placeholder='*******'
        />
    </div>

    <button
        type='submit'
        className='w-full text-white focus:ring-blue-300 font-medium rounded-lg
        text-sm px-5 py-2.5 text-center bg-brand-orange hover:bg-brand-orange-s
    '
    >
       Reset
    </button>
</form>
}
export default ForgotPassword;