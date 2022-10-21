import {useState} from 'react';
import axios from 'axios';
import { toast} from 'react-toastify';

import './style.css';

export const FileValidator = ({onSuccess}) => {
    const [files, setFiles] = useState([]);

    const onInputChange = (e) => {
        setFiles(e.target.files)
    };

    const onSubmit = (e) => {
        e.preventDefault();

        const data = new FormData();

        for(let i = 0; i < files.length; i++) {
            data.append('file', files[i]);
        }
        
        axios.post('//localhost:8000/upload', data)
            .then((response) => {
                toast.success('Upload Success');
                onSuccess(response.data)
            })
            .catch((e) => {
                toast.error('Upload Error')
            })
    };

    return (
        <div>
        <h1>File Validator</h1>
        <p>Welcome to your smart file validator</p>
        <p>To get started, please follow the instruction given below,</p>
        <ul>
          <li><strong>Upload</strong>. Upload your zip file to the app</li>
          <li><strong>Validate</strong>. Validate the file</li>
          <li><strong>Save</strong>. Save the file</li>
        </ul>
        <form method="post" action="#" id="#" onSubmit={onSubmit}>
            <div className="form-group files">
                <label>Upload Your File </label>
                <input type="file"
                       onChange={onInputChange}
                       className="form-control"
                       multiple/>
            </div>

            <button>Save</button>
        </form>
        </div>
    )
};