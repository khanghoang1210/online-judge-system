import React from 'react';
import Slider from '@/components/Slider/Slider'
import { useRouter } from 'next/router';

export default function PostDetail() {
  const router = useRouter();
  const {id} = router.query

  
  // Fake data
  const post = {
    id:{id},
    title: `Dynamic Programming`,
    content: `Nội dung chi tiết của bài post ${id}.`,
    difficulty: 'Trung bình',
    category: 'Thuật toán',
    solution: 'Nội dung giải pháp nằm ở đây...',
  };

  return (
      <div className='container mx-auto  mt-4'>
        <Slider title='Featured'/>
    <div className='w-3/4 bg-white p-8 shadow-md ml-4'>
      <h1 className='text-3xl font-bold text-gray-800'>{post.title}</h1>
      <p className='mt-4 text-gray-700'>ID hiện ở đây nè :{post.content}</p>
      <div className='mt-6'>
        <h2 className='text-xl font-semibold text-gray-800'>Độ khó: {post.difficulty}</h2>
        <h2 className='text-xl font-semibold text-gray-800'>Danh mục: {post.category}</h2>
        <h2 className='text-xl font-semibold text-gray-800'>Giải pháp:</h2>
        <p className='mt-2 text-gray-700'>{post.solution}</p>
      </div>
    </div>
  </div>

  );
}
