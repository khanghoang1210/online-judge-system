import { authModalState } from '@/atoms/authModalAtom';
import { error } from 'console';
import { useRouter } from 'next/router';
import React, { useEffect, useState } from 'react';
import { useSetRecoilState } from 'recoil';

type SignupProps = {
    
};

const API_URL = 'http://localhost:5107/api/Accounts/Create';
const Signup:React.FC<SignupProps> = () => {
    
    const setAuthModalState = useSetRecoilState(authModalState);
    const handleClick = ()=>{
        setAuthModalState((prev)=>({...prev, type:'login'}))
    }

    const  [inputs, setInputs] = useState({email:"",userName:"",fullName:"",password:""})
    const router = useRouter();
    const handleChangeInput = (e:React.ChangeEvent<HTMLInputElement>)=>{
        setInputs((prev) => ({...prev,[e.target.name]:e.target.value}));
    }
    const handleRegister = async (e:React.FormEvent<HTMLFormElement>)=>{
        e.preventDefault();
        if(!inputs.email || !inputs.userName ||!inputs.fullName || !inputs.password) return alert("Please fill all field");
        console.log(inputs)
        try{
            const data = {userName: inputs.userName, password: inputs.password, email:inputs.email, fullName:inputs.fullName}
            const res = await fetch(API_URL, {
                method: "POST",
                body: JSON.stringify(data),
                mode: "cors",
                headers: {
                    'Accept': 'application/json, text/plain',
                    'Content-Type': 'application/json;charset=UTF-8'
                },
              }) ;
            if(!res.ok) throw Error(res.statusText);
            const result = await res.json();
            if(result.StatusCode == 200) router.push("/auth");
            alert(result.message)

        }
        catch(error:any){
            alert(error.message)
        }
      
    };
    console.log(inputs)
    return <form className='space-y-4 px-6 pb-4' onSubmit={handleRegister}>
    <h3 className='text-xl font-medium text-white'>Register to CodeFast</h3>
    <div>
        <label htmlFor='email' className='text-sm font-medium block mb-2 text-gray-300'>
            Email
        </label>
        <input
            onChange={handleChangeInput}
            type='email'
            name='email'
            id='email'
            className='
border-2 outline-none sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5
bg-gray-600 border-gray-500 placeholder-gray-400 text-white
'
            placeholder='name@company.com'
        />
    </div>
    <div>
        <label htmlFor='displayName' className='text-sm font-medium block mb-2 text-gray-300'>
            User Name
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
            placeholder='John Doe'
        />
    </div>
    <div>
        <label htmlFor='displayName' className='text-sm font-medium block mb-2 text-gray-300'>
            Full Name
        </label>
        <input
            onChange={handleChangeInput}
            type='fullName'
            name='fullName'
            id='fullName'
            className='
border-2 outline-none sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5
bg-gray-600 border-gray-500 placeholder-gray-400 text-white
'
            placeholder='John Doe'
        />
    </div>
    <div>
        <label htmlFor='password' className='text-sm font-medium block mb-2 text-gray-300'>
            Password
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
      Sign Up
    </button>

    <div className='text-sm font-medium text-gray-300'>
        Already have an account?{" "}
        <a href='#' className='text-blue-700 hover:underline' onClick={()=>handleClick()}>
            Log In
        </a>
    </div>
</form>
}
export default Signup;