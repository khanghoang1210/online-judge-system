import { authModalState } from '@/atoms/authModalAtom';
import { Cookie } from 'next/font/google';
import { useRouter } from 'next/router';
import React from 'react';
import { useSetRecoilState } from 'recoil';
import { CookiesProvider, useCookies } from 'react-cookie'

type LoginProps = {
    
};
const API_URL = 'https://localhost:7004/api/Accounts/Login';
const Login:React.FC<LoginProps> = () => {

    const setAuthModalState = useSetRecoilState(authModalState);
    const handleClick = (type:"login"|"register"|"forgotPassword") =>{
        setAuthModalState((prev) =>({...prev, type}));
    }
    const router = useRouter();
    const [inputs, setInputs] = React.useState({userName:"", password:""});
    const handleChangeInput = (e:React.ChangeEvent<HTMLInputElement>)=>{
        setInputs((prev)=>({...prev,[e.target.name]:e.target.value}));

    }

    const [cookies, setCookie] = useCookies(['token'])

    const handleLogin = async(e:React.FormEvent<HTMLFormElement>)=>{
        e.preventDefault();
        if(!inputs.userName || !inputs.password) return alert("Please fill all field");
        try {
            const data = {userName: inputs.userName, password: inputs.password}
            const res = await fetch(API_URL, {
                method: "POST",
                body: JSON.stringify(data),
                mode: "cors",
                headers: {
                    'Accept': 'application/json, text/plain',
                    'Content-Type': 'application/json;charset=UTF-8'
                },
              });
            if(!res.ok) throw Error(res.statusText);
            const result = await res.json();
 
            if(result.statusCode == 200) {
                const token = result.data.accessToken;
                setCookie('token', token, { path: '/' })

            }
            
           
            
        } catch (error:any) {
            alert(error.message)
        }
    }
    return <form className='space-y-6 px-6 pb-4' onSubmit={handleLogin}>
    <h3 className='text-xl font-medium text-white'>Sign in to LeetClone</h3>
    <div>
        <label htmlFor='userName' className='text-sm font-medium block mb-2 text-gray-300'>
            Your User Name
        </label>
        <input
           onChange={handleChangeInput}
            type='userName'
            name='userName'
            id='userName'
            className='
    border-2 outline-none sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5
    bg-gray-600 border-gray-500 placeholder-gray-400 text-white
'
            placeholder='name@company.com'
        />
    </div>
    <div>
        <label htmlFor='password' className='text-sm font-medium block mb-2 text-gray-300'>
            Your Password
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

    <button
        type='submit'
        className='w-full text-white focus:ring-blue-300 font-medium rounded-lg
        text-sm px-5 py-2.5 text-center bg-brand-orange hover:bg-brand-orange-s
    '
    >
        Login
    </button>
    <button className='flex w-full justify-end' onClick={()=>handleClick("forgotPassword")}>
        <a href='#' className='text-sm block text-brand-orange hover:underline w-full text-right'>
            Forgot Password?
        </a>
    </button>
    <div className='text-sm font-medium text-gray-300'>
        Not Registered?{" "}
        <a href='#' className='text-blue-700 hover:underline' onClick={()=>handleClick("register")}>
            Create account
        </a>
    </div>
</form>
}
export default Login;