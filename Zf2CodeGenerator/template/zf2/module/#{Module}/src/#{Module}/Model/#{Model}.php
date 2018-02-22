<?php
/**
 * Created by Zf2CG
 * User: 
 * Date: #{Date}
 * Time: #{Time}
 */

namespace #{Module}\Model;

use Zf2Helper\Model\BaseModel;

class #{Model} extends BaseModel{

	// ----------
	// Property
	// ----------
	#{properties}

	// ----------
	// Method
	// ----------
	public function setId($id)
	{
		$this->id = $id;
		return $this;
	}
	public function getId()
	{
		return $this->id;
	}

	#{methods}
}
