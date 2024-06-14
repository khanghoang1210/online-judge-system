import { Post } from '@/components/Slider/Slider';
import Topbar from '@/components/Topbar/Topbar';
import useHasMounted from '@/hooks/useHasMounted';
import { useRouter } from 'next/router';
import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';

type PostPageProps = {
    //post:Post
};

const PostPage:React.FC<PostPageProps> = () => {
    const hasMounted = useHasMounted();
    const [post, setPost] = useState<Post | null>(null);
    const router = useRouter();
  
   
    
    useEffect(() => {
        const fetchProblemDetails = async () => {
          const { postId } = router.query;
          if (!postId) return; // Ensure postId is defined
    
          console.log(postId);
    
          try {
            const response = await fetch(`http://localhost:5107/api/Posts/${postId}`, {
              method: "GET",
              mode: "cors",
              headers: {
                Accept: "application/json, text/plain",
                "Content-Type": "application/json;charset=UTF-8",
              },
            });
    
            if (!response.ok) {
              throw new Error('Network response was not ok');
            }
    
            const data = await response.json();
            setPost(data.data);
          } catch (error) {
            toast.error("Failed to fetch problem details");
          }
        };
    
        fetchProblemDetails();
      }, [router.query]);

  if (!hasMounted) {
    console.log(hasMounted);
    return null;
  }
    
    
  return (
    <>
      <Topbar/>
    <div className="container mx-auto  mt-4">
      <div className="w-3/4 bg-white p-8 shadow-md ml-4">
        <h1 className="text-3xl font-bold text-gray-800">{post?.introduction}</h1>
        <p className="mt-4 text-gray-700"></p>
        <div className="mt-6">
        <div dangerouslySetInnerHTML={{__html: post?.content!.replace("`","'")!}} />
          {/* <h2 className="text-xl font-semibold text-gray-800">
            Độ khó: {post.difficulty}
          </h2>
          <h2 className="text-xl font-semibold text-gray-800">
            Danh mục: {post.category}
          </h2>
          <h2 className="text-xl font-semibold text-gray-800">Giải pháp:</h2>
          <p className="mt-2 text-gray-700">{post.solution}</p> */}
        </div>
      </div>
    </div>
    </>
  );
}
export default PostPage;